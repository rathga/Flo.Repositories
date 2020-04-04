<template>
    <nav class="navbar navbar-expand-md navbar-light bg-light fixed-top">
        <a class="navbar-brand" href="#">GoodToDo</a>
        <ConnectedApiStatus />
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav ml-auto" v-if="!loggedIn">
                <li class="nav-item">
                    <router-link to="/Login" class="nav-link" data-toggle="collapse" data-target=".navbar-collapse.show">Login</router-link>
                </li>
                <li class="nav-item">
                    <router-link to="/Register" class="nav-link" data-toggle="collapse" data-target=".navbar-collapse.show">Register</router-link>
                </li>
            </ul>
            <ul class="navbar-nav ml-auto" v-else>
                <li class="nav-item">
                    <a href="#" class="nav-link" @click="logoff" data-toggle="collapse" data-target=".navbar-collapse.show">Logoff</a>
                </li>
            </ul>
        </div>
    </nav>
</template>

<script lang="ts">
    import { Component, Prop, Vue, Emit } from 'vue-property-decorator';
    import ConnectedApiStatus from "./connected/ConnectedApiStatus.vue";

    @Component({
        components: { ConnectedApiStatus }
    })
    export default class Navbar extends Vue {
        @Prop() loggedIn!: boolean;
        @Emit() logoff() { };
    }
</script>

<style>
    body {
        padding: 60px 15px 0;
    }
</style>