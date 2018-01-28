var sock;
var statusMsg = document.getElementById('status')
var header = document.getElementById('header')
var logo = document.getElementById('logo')
var app = document.getElementById('app')
var title = ''
var currentRound;
var round;
var playerIcon;

var sentenceRound = false;

var data = {
  playerName: null,
  messageType: null,
  message: null
}

var ipAddressRegex = /(\d){1,3}.(\d){1,3}.(\d){1,3}.(\d){1,3}/

function init() {
  hideAll()
  logo.src = 'img/writersflocklogo.png'
  serverConnect.display()
}

var serverConnect = function () {
  var ipAddress = ''
  var element = document.getElementById('server-connect')
  var serverInput = document.getElementById('server-input')
  var nameInput = document.getElementById('name-input')
  var connectButton = document.getElementById('connect-button')

  function display() {
    element.classList.remove('hidden')
    connectButton.removeAttribute('disabled')
  }

  function hide() {
    element.classList.add('hidden')
  }

  function username() {
    return nameInput.value
  }

  function serverAddress() {
    return ipAddress
  }

  function init() {
    connectButton.addEventListener('click', connectServer, false)

    var _ipAddress = window.localStorage.getItem('serverAddress')
    var _username = window.localStorage.getItem('username')
    if (_ipAddress !== undefined) { serverInput.value = _ipAddress }
    if (_username !== undefined) { nameInput.value = _username }

    header.innerText = title

    console.log('server init')
  }
  init()

  function connectServer(e) {
    ipAddress = serverInput.value
    var port = 1024;
    if (ipAddress.match(ipAddressRegex)) {
      setStatus('trying to connect to ' + ipAddress + ':' + port)
      connectButton.setAttribute('disabled', '')
      sock = new WebSocket('ws://' + ipAddress + ':' + port)
      sock.addEventListener('open', onConnect, false)
      sock.addEventListener('message', onMessage, false)
      sock.addEventListener('error', onError, false)
      sock.addEventListener('close', onClose, false)
    } else {
      setStatus('incorrect format for ip address!')
    }
  }

  return {
    display: display,
    hide: hide,
    serverInput: serverInput,
    username: username,
    serverAddress: serverAddress
  }
}()

var voting = function () {
  var element = document.getElementById('voting')

  function display(options, textCount) {
    element.classList.remove('hidden')
    removeAllChildren(element)
    var ul = document.createElement('ul')
    for (var i = 0; i < options.length; i++) {
      var li = document.createElement('li')
      if (i < textCount) {
        li.textContent = options[i]
      } else {
        var btn = document.createElement('button')
        btn.textContent = options[i]
        btn.setAttribute('data-option', options[i])
        btn.addEventListener('click', vote, false)
        li.appendChild(btn)
      }
      ul.appendChild(li)
    }
    element.appendChild(ul)
  }

  function hide() {
    element.classList.add('hidden')
  }

  function vote(e) {
    var buttons = element.getElementsByTagName('button')
    for (var i = 0; i < buttons.length; i++) {
      buttons[i].setAttribute('disabled', '')
      if (buttons[i] !== e.target) {
        buttons[i].classList.add('strike')
      }
    }
    data.messageType = 2
    console.log(e.target.dataset.option)
    data.message = e.target.dataset.option
    sendData(data)
    e.target.innerText += ' ✔︎'
  }

  return {
    display: display,
    hide: hide
  }
}()

var entry = function () {
  var element = document.getElementById('entry')
  var submitBtn = document.getElementById('send')
  var input = document.getElementById('entry-input')
  var entryDisplay = document.getElementById('message')
  var endingDisplay = document.getElementById('ending')

  submitBtn.addEventListener('click', sendMessage, false)

  function display(message) {
    if (round !== '2') {
      entryDisplay.innerText = 'Last line: ' + message[message.length - 1]
      endingDisplay.classList.add('hidden')
    } else {
      endingDisplay.innerText = 'Ending line: ' + message[message.length - 1]
      entryDisplay.innerText = 'Last line: ' + message[message.length - 2]
      endingDisplay.classList.remove('hidden')
    }
    if (message[message.length - 1] === undefined) {
      entryDisplay.classList.add('hidden')
    }
    element.classList.remove('hidden')
  }

  function hide() {
    element.classList.add('hidden')
  }

  function sendMessage(message) {
    var m = input.value
    m = m.trim()
    m = m.replace(/\s\s+/g, ' ')
    if (m.length > 0) {
      data.message = m
      data.messageType = 1
      sendData(data)
      clearInput(input)
      hideAll()
      wait.display('waiting for all other players')
    }
  }

  return {
    display: display,
    hide: hide
  }
}()

var wait = function () {
  var element = document.getElementById('wait')

  function display(message) {
    // element.textContent = message
    element.classList.remove('hidden')
  }

  function hide() {
    element.classList.add('hidden')
  }

  return {
    display: display,
    hide: hide
  }
}()

var host = function () {
  var element = document.getElementById('host')
  var readyBtn = document.getElementById('ready')
  var restartElem = document.getElementById('restartSection')
  var restartBtn = document.getElementById('restart')
  var quitBtn = document.getElementById('quit')

  restartBtn.addEventListener('click', restartGame, false)
  quitBtn.addEventListener('click', quitGame, false)

  readyBtn.addEventListener('click', ready, false)

  function display() {
    element.classList.remove('hidden')
  }

  function hide() {
    element.classList.add('hidden')
    restartElem.classList.add('hidden')
  }

  function ready() {
    console.log('ready!')
    data.messageType = 5
    sendData(data)
  }

  function restart() {
    restartElem.classList.remove('hidden')
  }
  function restartGame() {
    data.messageType = 3
    sendData(data)
    hideAll()
  }
  function quitGame() {
    data.messageType = 4
    sendData(data)
    hideAll()
  }

  return {
    display: display,
    hide: hide,
    restart: restart
  }
}()

function setStatus(text) {
  console.log(text)
  statusMsg.textContent = text
}

function sendData(data) {
  console.log('%c→ sent', 'color: #f55')
  console.log(data)

  sock.send(JSON.stringify(data))
}

function onMessage(e) {
  hideAll()
  m = JSON.parse(e.data)

  header.innerText = (m.messageTitle === null) ? title : m.messageTitle
  currentRound++;

  if (m.numberOfWritingRounds !== null) {
    setStatus('Rounds left: ' + (m.numberOfWritingRounds - currentRound))
  }

  console.log('%c← got', 'color: #55f')
  console.log(m)

  if (m.messageType === 0) {
    setStatus('connected to game!')
    sentenceRound = true
    currentRound = 0;
    playerIcon = m.message[0]
    logo.src = 'img/characters/' + playerIcon + '.png'
    if (m.message.includes('host')) {
      host.display()
    } else {
      wait.display()
    }
  } else if (m.messageType === 1) {
    entry.display(m.message)
  } else if (m.messageType === 2) {
    round = ''
    if (m.message[0] === 'Once upon a time...') {
      voting.display(m.message, 2)
    } else {
      voting.display(m.message)
    }
  } else if (m.messageType === 4) {
    host.restart()
  } else if (m.messageType === 5) {
    round = m.message[0]
  } else if (m.messageType === 6) {
    wait.display()
  }

}

function onConnect() {
  console.log('connected!')
  window.localStorage.setItem('serverAddress', serverConnect.serverAddress())
  window.localStorage.setItem('username', serverConnect.username())

  hideAll()
  data.playerName = serverConnect.username().trim()
  data.messageType = 0

  sendData(data)

  wait.display('Connecting to server...')

  setStatus('connected to server, waiting for response...')
}

function onError(e) {
  setStatus('there was an issue connecting with this server, double check your connection and ip address')
}

function onClose() {
  setStatus('connection to server lost')
  hideAll()
  serverConnect.display()
  // clearInput(serverConnect.serverInput)
}

function removeAllChildren(parent) {
  while (parent.firstChild) {
    parent.removeChild(parent.firstChild);
  }
}

function clearInput(element) {
  element.value = ''
}

function hideAll() {
  entry.hide()
  voting.hide()
  serverConnect.hide()
  wait.hide()
  host.hide()
}




init()


