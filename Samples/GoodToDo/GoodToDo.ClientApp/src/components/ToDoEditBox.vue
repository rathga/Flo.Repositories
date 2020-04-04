<template>
    <div class="input-group mb-1">
        <div class="input-group-prepend">
            <div class="input-group-text">
                <input type="checkbox" :disabled="disabled" aria-label="Completed" v-model="editToDo.completed" @change="onCompletedChange(editToDo)">
            </div>
        </div>
        <input type="text" :disabled="disabled" class="form-control" 
               aria-label="ToDo Message"
               ref="input"
               :class="{ strike: editToDo.completed, 'is-invalid': notValid }"
               v-model="editToDo.message"
               @focus="onSelected(toDo)"
               @input="editStart"
               @keyup.enter="trySubmit"
               @keyup.esc="onEditCancel">
        <div v-if="selected" class="input-group-append">
            <button class="btn btn-light" type="button"
                    @click="trySubmit"><span class="oi oi-check"></span></button>
            <button class="btn btn-light" type="button"
                    @click="onEditCancel"><span class="oi oi-x"></span>
            </button>
            <button class="btn btn-light" type="button"
                    @click="onDelete" ref="deleteButton"><span class="oi oi-trash"></span></button>
        </div>
    </div>
</template>

<script lang="ts">
    import { Component, Prop, Emit } from 'vue-property-decorator';
    import ToDo from '../state/ToDo';
    import MessageInput from './MessageInput';

    @Component
    export default class ToDoItem extends MessageInput {

        // props
        @Prop() toDo!: ToDo;
        @Prop() disabled: boolean = false;
        @Prop() selected: boolean = false; 

        // events

        @Emit() onSelected(toDo: ToDo) { }

        @Emit() onEditStart() {
            this.modified = true;
        }
        @Emit() onEditCancel() {
            this.editToDo.message = this.toDo.message;
            this.modified = this.notValid = false;
            (<HTMLElement>this.$refs.input).blur();
        }
        @Emit() onCompletedChange(toDo: ToDo) { }

        @Emit() onEdited(message: string) {
            this.modified = false;
            (<HTMLElement>this.$refs.input).blur();
        }
        @Emit() onDelete() { }

        // data

        editToDo: ToDo = new ToDo(); // non-prop to do for modification
        modified: boolean = false; // true when the todo message has been modified

        // methods

        editStart() {
            // emit onEditStart if we are in focus and have changed the message for the first time
            if (this.editToDo!.message != this.toDo.message && !this.modified) {
                this.onEditStart();
            }

            // validate
            this.validateMessage(this.editToDo.message);
        }

        trySubmit() {
            if (this.validateMessage(this.editToDo.message)) this.onEdited(this.editToDo.message);
        }

        // life cycle

        mounted() {
            this.editToDo = { ...this.toDo }; // immutable copy for editing
        }
    }
</script>

<style scoped>
    .strike { text-decoration-line: line-through; }
    .btn-light {
        background-color: #e9ecef;
        border-color: rgb(206, 212, 218);
    }
</style>