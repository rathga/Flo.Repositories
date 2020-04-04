<template>
    <div>
        <div v-if="loading"> <span class="spinner-border spinner-border-sm"></span> </div>
        <div v-else class="badge badge-danger">{{ message }}</div>
        <transition name="fade">
            <div v-if="toast != ''" class="badge badge-success">{{ toast }}</div>
        </transition>

    </div>
</template>

<script lang="ts">
    import { Component, Vue, Prop } from 'vue-property-decorator';

    @Component
    export default class ApiStatus extends Vue {
        @Prop() loading!: boolean;
        @Prop() message!: string;
        @Prop() toast!: string;
    }
</script>

<style scoped>
    .fade-enter-active, .fade-leave-active {
        transition: opacity .2s;
    }

    .fade-enter, .fade-leave-to /* .fade-leave-active below version 2.1.8 */ {
        opacity: 0;
    }
</style>