import IState from './State';
import ToDo from './ToDo';
import { AccountClient, ToDoClient, Result, ApiException, ToDoItem } from '../apiclient/ApiClient';
import VueRouter from 'vue-router';
import Config from '../../config';

// ToDoService: state control & api access

// TODO: consider refactoring to multiple smaller classes

class ToDoService {

    constructor(router: VueRouter) {
        this.router = router;
    }

    private readonly accClient = new AccountClient(Config.value("apiUrl")); 
    private readonly todoClient = new ToDoClient(Config.value("apiUrl"));
    private readonly router: VueRouter;

    // ** state **

    // set initial state
    private static _state: IState = {
        apiError: null, apiLoading: false, apiMessage: '',
        registerResult: null, registerLoading: false,
        userId: null,
        loginResult: null, loginLoading: false,
        toDos: [],
        toasts: []
    };

    get state(): IState { return ToDoService._state; }

    // state updated event
    private static handlers: { (newState: IState): void }[] = [];

    subscribe = (handler: (newState: IState) => void) => {
        ToDoService.handlers.push(handler);
    }

    unsubscribe = (handler: (newState: IState) => void) => {
        ToDoService.handlers.filter(h => h != handler);
    }

    updateState = (newState: IState) => {
        ToDoService._state = newState;
        ToDoService.handlers.forEach(h => h(newState));
    }

    // ** service methods **

    // wrap all api calls with some API state control and handling
    private callApi = async (apiMethod: () => Promise<void>) => {
        this.reducers.startApiCall();
        try {
            await apiMethod();
        }
        catch (e) {
            this.reducers.apiError(<Error>e);
        }
        finally {
            this.reducers.stopApiCall();
        }
    }

    // toasts

    toast = (message: string) => {
        this.reducers.addToast(message);
        setTimeout(() => this.reducers.removeOldestToast(), 1000);
    }

    // routing

    redirectTo = (to: string) => this.router.push(to);

    // register 

    register = async (name: string, password: string) => {
        this.reducers.setRegisterLoading(true);
        await this.callApi(async () => {
            let r = await this.accClient.register(name, password);
            if (r.success) this.isLoggedIn(); // if user sucessfully registered, they will now be logged in.  set userid
            this.reducers.registerResponse(r);
        });
    }

    clearRegisterState = () => this.reducers.clearRegisterState();

    //login

    isLoggedIn = async (): Promise<boolean> => {
        let result: boolean = false;
        await this.callApi(async () => {
            try {
                let userId = await this.accClient.getCurrentUserId();
                result = userId != null;
                this.reducers.setUserId(userId);
            }
            catch (e) {
                this.reducers.setUserId(null);
                if (e instanceof ApiException) {
                    let ae = e as ApiException;
                    if (ae.status == 401) result = false;                   
                }
                else throw (e);
            }
        });
        return result;
    }

    clearLoginState = () => this.reducers.clearLoginState();

    login = async (name: string, password: string) => {
        this.reducers.setLoginLoading(true);
        await this.callApi(async () => {
            this.reducers.loginResponse(await this.accClient.login(name, password));
            if (this.state.loginResult!.success) {
                await this.isLoggedIn();
                this.redirectTo("/");
              }
        });
    }

    logoff = async () => {
        await this.callApi(async () => {
            this.reducers.setUserId(null);
            try {
                await this.accClient.logOff();
                this.redirectTo("/home");
            }
            catch (e) {
                if (!(e instanceof ApiException)) throw (e);
            }
        });
    }

    // todos

    requestToDos = async () => {
        await this.callApi(async () => {
            if (this.state.userId == null)
                this.redirectTo("/home");
            else
                await this.reducers.receiveToDos(await this.todoClient.getForUser(this.state.userId));
        });
    }

    addToDo = async (toDo: ToDo) => {
        await this.callApi(async () => {
            let item: ToDoItem = new ToDoItem({ message: toDo.message, complete: false, userId: this.state.userId!, id: undefined });
            let result = await this.todoClient.add(item);
            if (result.success) {
                this.reducers.addToDo({ ...toDo, id: result.value });
                this.toast("ToDo added!");
            }
        });
    }

    updateToDo = async (toDo: ToDo) => {
        await this.callApi(async () => {
            let r = await this.todoClient.update(new ToDoItem({ message: toDo.message, complete: toDo.completed, userId: this.state.userId!, id: toDo.id }));
            if (r.success) {
                this.reducers.updateToDo(toDo);
                this.toast("ToDo updated!");
            }
        });
    }

    deleteToDo = async (toDo: ToDo) => {
        await this.callApi(async () => {
            await this.todoClient.delete(toDo.id);
            this.reducers.deleteToDo(toDo);
            this.toast("ToDo deleted!");
        });
    }

    //  ** reducers **

    private reducers = new ToDoService.Reducers(this.updateState, this);

    static Reducers = class {

        private updateState: (newState: IState) => void;
        private service: ToDoService;

        constructor(updateState: (newState: IState) => void, service: ToDoService) {
            this.updateState = updateState;
            this.service = service;
        }


        // toasts

        addToast = (message: string) => {
            this.updateState({ ...this.service.state, toasts: [ ...this.service.state.toasts, message ] })
        }

        removeOldestToast = () => {
            this.updateState({ ...this.service.state, toasts: [...this.service.state.toasts.splice(1) ] })
        }

        // clearers

        clearRegisterState = () => {
            this.updateState({ ...this.service.state, registerResult: null })
        }

        clearLoginState = () => {
            this.updateState({ ...this.service.state, loginResult: null })
        }

        // api

        apiError = (e: Error) => {
            this.updateState({ ...this.service.state, apiError: e, apiMessage: "Server error: try again?" });
        }

        startApiCall = () => {
            this.updateState({ ... this.service.state, apiLoading: true, apiMessage: "" });
        }

        stopApiCall = () => {
            this.updateState({ ... this.service.state, apiLoading: false, registerLoading: false, loginLoading: false });
        }

        // todos

        receiveToDos = (toDos: ToDoItem[]) => {
            this.updateState({ ...this.service.state, toDos: toDos.map(t => { return { id: t.id, message: t.message, completed: t.complete } }) })
        }

        addToDo = (toDo: ToDo) => {
            this.updateState({ ...this.service.state, toDos: [...this.service.state.toDos, toDo ] });
        }

        deleteToDo = (toDo: ToDo) => {
            this.updateState({ ...this.service.state, toDos: this.service.state.toDos.filter((t) => t.id != toDo.id) });
        }

        updateToDo = (toDo: ToDo) => {
            this.updateState({ ...this.service.state, toDos: this.service.state.toDos.map((t) => t.id == toDo.id  ? toDo : t) });
        }


        // register

        setRegisterLoading = (loading: boolean) => {
            this.updateState({ ...this.service.state, registerLoading: loading });
        }

        registerResponse = (result: Result) => {
            this.updateState({ ...this.service.state, registerResult: result, registerLoading: false })
        }

        // login

        setUserId = (id: string | null) => {
            this.updateState({ ...this.service.state, userId: id });
        }

        setLoginLoading = (loading: boolean) => {
            this.updateState({ ...this.service.state, loginLoading: loading });
        }

        loginResponse = (result: Result) => {
            this.updateState({ ...this.service.state, loginResult: result, loginLoading: false });
        }
    }
    
}

export default ToDoService;