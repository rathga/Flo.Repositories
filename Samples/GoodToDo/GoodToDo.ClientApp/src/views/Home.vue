<template>
    <div class="container">

        <div class="jumbotron text-center">
            <p>Welcome to GoodToDo!  The world's newest and most basic ToDo list app.</p>
            <p>To get started, register or login using the menu above.</p>
        </div>

    </div>
</template>

<script lang="ts">
    import { Component, Prop } from 'vue-property-decorator';
    import ConnectedComponent from '../state/ConnectedComponent';

    @Component
    export default class Home extends ConnectedComponent {

        // props

        @Prop() private msg!: string;

        // life cycle

        async created() {
            // if we are logged in, go to the todos instead
            if (await this.service.isLoggedIn()) this.service.redirectTo("/");
        }
    }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
