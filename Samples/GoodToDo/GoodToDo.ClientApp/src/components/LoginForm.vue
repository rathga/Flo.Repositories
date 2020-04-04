<template>
    <form v-if="result == null || result.failure">
        <div class="d-flex justify-content-center">

            <div>
                <div class="text-center" v-if="result != null && result.errors">
                    <ul>
                        <li class="text-danger" v-for="e in result.errors.filter(e => e.key == '')">
                            {{ e.message }}
                        </li>
                    </ul>
                </div>
                <div class="input-group  mb-1">
                    <div class="input-group-prepend">
                        <div class="input-group-text">
                            Username
                        </div>
                    </div>
                    <input type="text" class="form-control" v-model="username"
                           :class="{ 'is-valid': isValid('Email'), 'is-invalid': isInvalid('Email') }" />
                    <div class="invalid-feedback" v-if="isInvalid('Email')">
                        <ul>
                            <li v-for="e in result.errors.filter(e => e.key == 'Email')">
                                {{ e.message }}
                            </li>
                        </ul>
                    </div>
                </div>
                <div class="input-group mb-1">
                    <div class="input-group-prepend">
                        <div class="input-group-text">
                            Password
                        </div>
                    </div>
                    <input type="password" class="form-control" v-model="password"
                           :class="{ 'is-valid': isValid('Password'), 'is-invalid': isInvalid('Password') }" />
                    <div class="invalid-feedback" v-if="isInvalid('Password')">
                        <ul>
                            <li v-for="e in result.errors.filter(e => e.key == 'Password')">
                                {{ e.message }}
                            </li>
                        </ul>
                    </div>
                </div>
                <div class="text-center">
                    <button type="button" class="btn btn-primary" @click="login(username, password)" :disabled="loading">
                        <span v-if="loading" class="spinner-border spinner-border-sm" ></span> Login
                    </button>
                </div>
            </div>
        </div>
    </form>
</template>

<script lang="ts">
    import { Component, Vue, Prop, Emit } from 'vue-property-decorator';
    import Result from '../Result';

    @Component
    export default class LoginForm extends Vue {

        // props

        @Prop() result!: Result | null;   
        @Prop() loading!: boolean;

        // events

        @Emit() login(username: string, password: string) { }

        // data
        username: string = '';
        password: string = '';

        // methods

        isValid(key: string) {
            if (this.result == null) return false; 
            return this.result.errors!.filter(e => e.key == key).length == 0;
        }

        isInvalid(key: string) {
            if (this.result == null) return false;
            return this.result.errors!.filter(e => e.key == key).length > 0;
        }
    }
</script>

<style scoped>
</style>