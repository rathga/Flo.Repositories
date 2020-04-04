import Vue from 'vue';
import Router from 'vue-router';
import Home from './views/Home.vue';
import ToDos from './views/ToDos.vue';
import Register from './views/Register.vue';
import Login from './views/Login.vue';

Vue.use(Router)

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      name: 'todos',
      component: ToDos
    },
    {
        path: '/register',
        name: 'register',
        component: Register
    },
    {
        path: '/home',
        name: 'home',
        component: Home
      },
      {
          path: '/login',
          name: 'login',
          component: Login
      }
  ]
})
