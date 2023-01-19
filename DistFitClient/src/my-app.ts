import { IdentityService } from './services/IdentityService';
import { route } from 'aurelia';

@route({
  routes: [
    {
      id: 'home',
      path: '',
      component: import('./views/Home'),
      title: 'DistFit - Home',
    },
    {
      id: 'exercises',
      path: '/exercises',
      component: import('./views/exercises/Exercises'),
      title: 'DistFit - Exercises',
    },
    {
      id: 'addExercise',
      path: '/exercises/add',
      component: import('./views/exercises/AddExercise'),
      title: 'DistFit - Add Exercise',
    },
    {
      id: 'sets',
      path: '/sets/:id',
      component: import('./views/exercises/Sets'),
      title: 'DistFit - Sets',
    },
    {
      id: 'measurements',
      path: '/measurements',
      component: import('./views/measurements/Measurements'),
      title: 'DistFit - Measurements',
    },
    {
      id: 'addMeasurement',
      path: '/measurements/add',
      component: import('./views/measurements/AddMeasurement'),
      title: 'DistFit - Add Measurement',
    },
    {
      id: 'measurementGraph',
      path: '/measurements/graphs/:id/:confirmed?',
      component: import('./views/measurements/Measurements'),
      title: 'DistFit - Measurements',
    },
    {
      id: 'programs',
      path: '/programs',
      component: import('./views/programs/Programs'),
      title: 'DistFit - Programs',
    },
    {
      id: 'login',
      path: '/login',
      component: import('./views/identity/Login'),
      title: 'DistFit - Login',
    },
    {
      id: 'register',
      path: '/register',
      component: import('./views/identity/Register'),
      title: 'DistFit - Register',
    }
  ]
})

export class MyApp {

  constructor(private IdentityService: IdentityService) {
  }

  attached() {
    this.IdentityService.checkLocalStorageForIdentity();
  }
}
