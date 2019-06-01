var isRunning = false;
var clocktimer;
var currentTimerNumber;
var hours, minutes, seconds;

function startStop() {
    if (typeof currentTimerNumber === "undefined") {
        setCurrentTimerNumber(event.target.id);
        time = getCurrentTime(currentTimerNumber);
        seconds = time[2];
        minutes = time[1];
        hours = time[0];
        start();
    }
    else if (event.target.id.toString() !== "start-stop-timer-" + currentTimerNumber) {
        stop();
        setCurrentTimerNumber(event.target.id);
        time = getCurrentTime(currentTimerNumber);
        seconds = time[2];
        minutes = time[1];
        hours = time[0];
        start();
    }
    else {
        if (isRunning) stop();
        else start();
    }
}

function start() {
    let objectiveName = document.getElementById("name-" + currentTimerNumber).innerHTML;
    postRequestStart("https://localhost:44397/Timer/StartWatch", objectiveName);
    clocktimer = setInterval("update()", 1000);
    isRunning = true;
    document.getElementById("start-stop-timer-" + currentTimerNumber).innerHTML = "Stop";
}

function stop() {
    postRequestStop("https://localhost:44397/Timer/StopWatch");
    clearInterval(clocktimer);
    isRunning = false;
    document.getElementById("start-stop-timer-" + currentTimerNumber).innerHTML = "Start";
}

function setCurrentTimerNumber(targetId) {
    let targetIdString = targetId.toString();
    currentTimerNumber = targetIdString[targetIdString.length - 1];
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

function getCurrentTime(currentTimerNumber) {
    let id = "time-spent-" + currentTimerNumber;
    let time = document.getElementById(id).innerHTML;
    let timeSplit = time.split(":");

    return timeSplit;
}

function postRequestStop(url) {
    return fetch(url, {
        credentials: "same-origin",
        method: "POST"
    });
}

function postRequestStart(url, name) {
    return fetch(url + "?" + "name=" + name.toString(), {
        credentials: "same-origin",
        method: "POST"
    });
}