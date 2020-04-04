<template>
    <ApiStatus :loading="state.apiLoading" :message="state.apiMessage" :toast="latestToast()" />
</template>

<script lang="ts">
    import { Component } from 'vue-property-decorator';
    import ConnectedComponent from '../../state/ConnectedComponent';
    import ApiStatus from '../ApiStatus.vue';

    @Component({
        components: { ApiStatus }
    })
    export default class ConnectedApiStatus extends ConnectedComponent {
        latestToast() {
            if (this.state!.toasts.length > 0)
                return this.state!.toasts[0];
            else
                return '';
        }
    }
</script>

<style scoped>
</style>