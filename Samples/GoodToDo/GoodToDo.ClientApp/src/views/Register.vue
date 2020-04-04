<template>
    <RegisterForm @register="service.register"
                  :result="state.registerResult" 
                  :loading="state.registerLoading" />
</template>

<script lang="ts">
    import { Component, Vue, Prop, Emit } from 'vue-property-decorator';
    import RegisterForm from '../components/RegisterForm.vue';
    import ConnectedComponent from '../state/ConnectedComponent';

    @Component({
        components: {
            RegisterForm
        }
    })
    export default class Register extends ConnectedComponent {

        // life cycle

        async created() {
            // clear state, in case someone has previously registered
            this.service.clearRegisterState();

            // check if we are logged in, and redirect to ToDos if so
            if (await this.service.isLoggedIn()) this.$router.push("/");
        }

    }
</script>

<style scoped>
</style>