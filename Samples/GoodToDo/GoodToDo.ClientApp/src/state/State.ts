import ToDo from './ToDo';
import Result from '../Result';

// IState: holds all application state

interface IState {
    apiError: Error | null;
    apiLoading: boolean;
    apiMessage: string;

    toDos: ToDo[];

    registerResult: Result | null;
    registerLoading: boolean;

    loginResult: Result | null;
    loginLoading: boolean;

    userId: string | null;

    toasts: string[];

}

export default IState;