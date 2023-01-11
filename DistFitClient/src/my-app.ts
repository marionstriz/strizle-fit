import { route } from 'aurelia';

@route({
  routes: [
    {
      id: 'home',
      path: '',
      component: import('./views/home'),
      title: 'Home',
    },
    {
      id: 'measurements',
      path: '/measurements',
      component: import('./views/measurements'),
      title: 'Measurements',
    }
  ]
})

export class MyApp {}
