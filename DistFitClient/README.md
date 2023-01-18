# DistFitClient

This project is bootstrapped by [aurelia/new](https://github.com/aurelia/new).

## Start dev web server

    npm start

### Docker
Remove platform specification if building from amd machine.
~~~sh
docker buildx build --platform linux/amd64 -t distfit-client .
docker tag distfit-client marionstriz/distfit-client:latest
docker push marionstriz/distfit-client:latest
~~~