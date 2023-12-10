const app = require('express')();
const port = 3000;

const bodyparser = require('express').json;
app.use(bodyparser())
app.listen(port, ()=>{
    console.log('serve running')
})