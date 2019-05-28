var isRunning = false;
var clocktimer;
var currentTimerNumber;

function start() {
    if (event.target.id !== currentTimerNumber) {
        if (typeof currentTimerNumber !== "undefined") {
            let oldStopButton = document.getElementById("stop-timer-" + currentTimerNumber);
            let oldTimerName = document.getElementById("name-" + currentTimerNumber).innerHTML;
            oldStopButton.click();
            oldStopButton.removeEventListener("click", stop);
        }

        isRunning = false;
        clearInterval(clocktimer);
        let eventId = event.target.id.toString();
        currentTimerNumber = eventId[eventId.length - 1];
        document.getElementById("stop-timer-" + currentTimerNumber).addEventListener("click", stop);
        time = getCurrentTime(currentTimerNumber);
        seconds = time[2];
        minutes = time[1];
        hours = time[0];
    }
    if (!isRunning) {
        clocktimer = setInterval("update()", 1000);
        isRunning = true;
    }
}

function update() {
    seconds++;
    if (seconds >= 60) {
        seconds = 0;
        minutes++;
        if (minutes >= 60) {
            minutes = 0;
            hours++;
        }
    }
    document.getElementById("time-spent-" + currentTimerNumber).innerHTML = hours.toString() + ":" + minutes.toString() + ":" + String(seconds).padStart(2, '0');
}

function stop() {
    clearInterval(clocktimer);
    isRunning = false;
}

function getCurrentTime(currentTimerNumber) {
    let id = "time-spent-" + currentTimerNumber;
    let time = document.getElementById(id).innerHTML;
    let timeSplit = time.split(":");

    return timeSplit;
}

function postRequest(url, data) {
    return fetch(url, {
        method: "POST",
        body: data
    });
}