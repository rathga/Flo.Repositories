<template>
  <div id="app">
      <Navbar :loggedIn="state.userId != null" @logoff="service.logoff" />
    <router-view/>
      <Footer />

  </div>
</template>

<script lang="ts">
    import Navbar from './components/Navbar.vue';
    import Footer from './components/Footer.vue';
    import { Component, Vue } from 'vue-property-decorator';
    import ConnectedComponent from './state/ConnectedComponent';
    import Redirect from './components/Redirect.vue';

    @Component({
        components:
        {
            Navbar, Footer
        }
    })
    export default class App extends ConnectedComponent {

    }
        
</script>
<style>
</style>
