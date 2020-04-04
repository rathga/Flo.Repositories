import { Vue, Component } from 'vue-property-decorator';
import ToDoService from './ToDoService';
import IState from './State';

// ConnectedComponent: adds state and access to the service to a component

@Component
export default class ConnectedComponent extends Vue {
    service!: ToDoService;
    state: IState | null = null; // instantiate state here to include in data()

    private setState(newState: IState) { this.state = newState; }

    created() {
        this.service = new ToDoService(this.$router); // create service here to exclude from data() / reactivity (not needed, and problems with 'this' and recursion when deep-copying)
        this.state = this.service.state;
        this.service.subscribe( (newState: IState) => { this.setState(newState); } ); // subscribe to state changes
    }

    destroyed() {
        this.service.unsubscribe( (newState: IState) => { this.setState(newState); } )
    }
}