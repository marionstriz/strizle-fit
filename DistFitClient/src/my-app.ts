import { route } from 'aurelia';

@route({
  routes: [
    {
      id: 'home',
      path: '',
      component: import('./views/Home'),
      title: 'Home',
    },
    {
      id: 'measurements',
      path: '/measurements',
      component: import('./views/measurements'),
      title: 'Measurements',
    },
    {
      id: 'login',
      path: '/login',
      component: import('./views/identity/login'),
      title: 'Login',
    }
  ]
})

export class MyApp {}
