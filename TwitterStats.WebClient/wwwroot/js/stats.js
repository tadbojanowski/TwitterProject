"use strict";

var connectionStats = new signalR.HubConnectionBuilder().withUrl("/statsHub")
    //.configureLogging({
    //    log: function (logLevel, message) {
    //        console.log(new Date().toISOString() + ": " + message);
    //    }
    //})
    .build();
connectionStats.serverTimeoutInMilliseconds = 1000 * 60 * 10; // 1 second * 60 * 10 = 10 minutes.
 
//Disable send button until connectionStats is established
//document.getElementById("streamSeconds").disabled = true;

async function start() {
    try {
        await
connectionStats.start().then(function () {

    connectionStats.stream("Counter")
        .subscribe({
            next: (item) => {
                if (item != null && item.averages != null && item.stats != null) {

                    document.getElementById("TotalRecived").innerHTML = item.averages.totalRecived;
                    document.getElementById("PerHourAverage").innerHTML = item.averages.perHourAverage.toFixed(2);
                    document.getElementById("PerMinuteAverage").innerHTML = item.averages.perMinuteAverage.toFixed(2);
                    document.getElementById("PerSecondAverage").innerHTML = item.averages.perSecondAverage.toFixed(2);
                    document.getElementById("TopEmojis").innerHTML = item.stats.topEmojis;
                    document.getElementById("TopHashtags").innerHTML = item.stats.topHashtags;
                    document.getElementById("TopMentions").innerHTML = item.stats.topMentions;
                    document.getElementById("TopDomains").innerHTML = item.stats.topDomains;
                    document.getElementById("PercentEmojis").innerHTML = item.stats.percentEmojis.toFixed(2) + '%';
                    document.getElementById("PercentPics").innerHTML = item.stats.percentPics.toFixed(2) + '%';
                    document.getElementById("PercentUrls").innerHTML = item.stats.percentUrls.toFixed(2) + '%';

                    var lookingForData = document.getElementById("looking");
                    if (lookingForData != null) {
                        document.getElementById("messagesList").removeChild(lookingForData);
                        document.getElementById("alert").classList.add("hidden");
                        lookingForData = null;
                    }
                }
            },
            complete: () => {
                var li = document.createElement("li");
                li.textContent = "Stream completed";
                document.getElementById("messagesList").appendChild(li);
                document.getElementById("alert").classList.add("visible");
            },
            error: (err) => {
                var li = document.createElement("li");
                li.textContent = err;
                document.getElementById("messagesList").appendChild(li);
                document.getElementById("alert").classList.add("visible");
            },
        });
     
}).catch(function (err) {
    var li = document.createElement("li");
    li.textContent = err;
    document.getElementById("messagesList").appendChild(li);
    document.getElementById("alert").classList.add("visible");
    return console.error(err.toString());
});
 

    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

connectionStats.onclose(start);

start();