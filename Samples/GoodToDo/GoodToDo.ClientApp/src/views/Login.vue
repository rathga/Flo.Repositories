<template>
    <LoginForm @login="service.login"
                  :result="state.loginResult"
               :loading="state.loginLoading "/>
</template>

<script lang="ts">
    import { Component, Vue, Prop, Emit } from 'vue-property-decorator';
    import LoginForm from '../components/LoginForm.vue';
    import ConnectedComponent from '../state/ConnectedComponent';

    @Component({
        components: {
            LoginForm
        }
    })
    export default class Login extends ConnectedComponent {

        // life cycle

        async created() {
            // clear state, in case someone has previously logged in
            this.service.clearLoginState();

            // logout if we are logged in
            await this.service.logoff();
        }

    }
</script>

<style scoped>
</style>