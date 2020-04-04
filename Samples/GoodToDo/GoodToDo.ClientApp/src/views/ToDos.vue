<template>
    <ToDoList :toDos="state.toDos"
          @on-add="service.addToDo"
          @on-update="service.updateToDo"
          @on-delete="service.deleteToDo" />
</template>

<script lang="ts">
    import { Component, Vue} from 'vue-property-decorator';
    import ToDoList from '../components/ToDoList.vue';
    import ConnectedComponent from '../state/ConnectedComponent';

    @Component({
        components: {
            ToDoList
        }
    })
    export default class ToDos extends ConnectedComponent {

        // lifecycle

        async created() {
            // get some ToDos!
            await this.service.requestToDos();
        }
    }
</script>

<style scoped>
</style>