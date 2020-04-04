<template>
    <div>

        <ToDoInput @to-do-entered="added" 
                   class="mb-1" />

        <ToDoEditBox :toDo="toDo" v-for="toDo in toDos" :key="toDo.id"
                        @on-selected="selected"
                        @on-edit-start="editStart"
                        @on-edit-cancel="editCancel"
                        @on-edited="edited"
                        @on-delete="deleted"
                        @on-completed-change="onUpdate"
                        :disabled="editing == true && toDo != selectedToDo"
                        :selected="toDo == selectedToDo" />

    </div>
</template>

<script lang="ts">
    import { Component, Vue, Prop, Emit } from 'vue-property-decorator';
    import ToDoInput from './ToDoInput.vue';
    import ToDo from '../state/ToDo';
    import ToDoEditBox from './ToDoEditBox.vue';

    @Component({
        components: {
            ToDoInput,
            ToDoEditBox
        }
    })
    export default class ToDoList extends Vue {

        // props
        @Prop() toDos!: ToDo[];

        // events
        @Emit() onAdd(toDo: ToDo) { }
        @Emit() onUpdate(toDo: ToDo) { }
        @Emit() onDelete(toDo: ToDo) { }

        // methods
        added(message: string) {
            this.selectedToDo = null;
            this.onAdd({ message: message, completed: false, id: '' });
        }

        selectedToDo: ToDo | null = null; // the toDo seleted
        editing = false;


        selected(toDo: ToDo) {
            this.selectedToDo = toDo;
        }

        editStart() { // start editing
            this.editing = true;
        }

        editCancel() { // stop editing
            this.selectedToDo = null;
            this.editing = false;
        }

        edited(newMessage: string) { // submit editing
            this.selectedToDo!.message = newMessage;
            this.onUpdate(this.selectedToDo!);
            this.selectedToDo = null;
            this.editing = false;
        }

        deleted() {
            this.onDelete(this.selectedToDo!);
            this.selectedToDo = null;
            this.editing = false;
        }


    };
</script>

<style scoped>
</style>