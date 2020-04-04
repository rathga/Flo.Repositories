<template>
    <input type="text" placeholder="Enter your ToDo item and hit enter" 
           class="form-control w-100" ref="input"
           v-on:keyup.enter="trySubmitToDo" v-model="toDo"
           v-on:input="validateMessage(toDo)"
           :class="{ 'is-invalid': notValid }" />
</template>

<script lang="ts">
    import { Component, Emit } from 'vue-property-decorator';
    import MessageInput from './MessageInput';

    @Component
    export default class ToDoInput extends MessageInput {

        // events
        @Emit() toDoEntered(toDo: string) {
            this.toDo = '';
            this.notValid = false;
        }

        // data
        toDo: string = "";

        // methods

        trySubmitToDo() {
            if (this.validateMessage(this.toDo))
                this.toDoEntered(this.toDo);
        }

        // life cycle

        mounted() {
            (<HTMLElement>this.$refs.input).focus(); // give focus when text box appears
        }
    };
</script>

<style scoped>
</style>