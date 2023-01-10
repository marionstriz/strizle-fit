import 'jquery';
import 'bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import '../static/site.css';

import Aurelia from 'aurelia';
import { MyApp } from './my-app';

Aurelia
  .app(MyApp)
  .start();
