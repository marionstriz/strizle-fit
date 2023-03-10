import 'jquery';
import '@popperjs/core'
import 'bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import '../static/site.css';

import Aurelia, { RouterConfiguration } from 'aurelia';
import { MyApp } from './my-app';

Aurelia
  .register(RouterConfiguration)
  .app(MyApp)
  .start();
