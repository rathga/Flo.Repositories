import { Vue, Component } from 'vue-property-decorator';

// ConnectedComponent: adds validation helpers for todo.message editing components

@Component
export default class MessageInput extends Vue {

    // data

    notValid = false;

    // methods

    validateMessage(message: string) {
        if (this.isNullOrWhitespace(message)) {
            this.notValid = true;
            return false;
        }
        this.notValid = false;
        return true;
    }

    isNullOrWhitespace(input: string) {

        if (typeof input === 'undefined' || input == null) return true;
        return input.replace(/\s/g, '').length < 1;
    }
}