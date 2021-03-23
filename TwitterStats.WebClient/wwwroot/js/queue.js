"use strict";

var connectionQueue = new signalR.HubConnectionBuilder().withUrl("/queueHub").build();
connectionQueue.serverTimeoutInMilliseconds = 1000 * 60 * 10; // 1 second * 60 * 10 = 10 minutes.
//Disable send button until connectionQueue is established
//document.getElementById("sendButton").disabled = true; 

connectionQueue.start().then(function () {
    //document.getElementById("sendButton").disabled = false;
    
}).catch(function (err) {
    var li = document.createElement("li");
    li.textContent = err;
    document.getElementById("messagesList").appendChild(li);
    document.getElementById("alert").classList.add("visible");
    return console.error(err.toString());
});


document.getElementById("sendButton").addEventListener("click", function (event) {

    document.getElementById("sendButton").disabled = true; 
    document.getElementById("spinner").style.visibility = 'visible';

    var token = new CancellationTokenSource;    

    connectionQueue.invoke("QueueUpTweets", token).catch(function (err) {  
        var li = document.createElement("li");
        li.textContent = err;
        document.getElementById("messagesList").appendChild(li);
        document.getElementById("alert").classList.add("visible");
        return console.error(err.toString());
    });
    event.preventDefault();
});



const CANCEL = Symbol();

class CancellationToken {

    constructor() {
        this.cancelled = false;
    }

    throwIfCancelled() {
        if (this.isCancelled()) {
            throw "Cancelled!";
        }
    }

    isCancelled() {
        return this.cancelled === true;
    }

    [CANCEL]() {
        this.cancelled = true;
    }

    // could probably do with a `register(func)` method too for cancellation callbacks

}

class CancellationTokenSource {

    constructor() {
        this.token = new CancellationToken();
    }

    cancel() {
        this.token[CANCEL]();
    }

}