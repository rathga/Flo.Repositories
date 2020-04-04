<template>
    <form>
        <div v-if="result != null && result.success">
            <div class="jumbotron">
                <p>Thank you.  You are now registered and logged in!</p>
                <router-link to="/">Click here</router-link> to go to the todos!
            </div>
        </div>
        <div v-else class="d-flex justify-content-center">
            <div>
                <div class="input-group  mb-1">
                    <div class="input-group-prepend">
                        <div class="input-group-text">
                            Username
                        </div>
                    </div>
                    <input type="text" class="form-control" v-model="username" 
                           :class="{ 'is-valid': isValid('Email'), 'is-invalid': isInvalid('Email') }"  />
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
                    <button type="button" class="btn btn-primary" @click="register(username, password)" :disabled="loading">
                        <span v-if="loading" class="spinner-border spinner-border-sm"></span> Register
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
    export default class Register extends Vue {

        // props

        @Prop() result!: Result | null;   
        @Prop() loading!: boolean;

        // events

        @Emit() register(username: string, password: string) { }

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