var sock;
var input = document.getElementById('input')
var message = document.getElementById('message')
var button = document.getElementById('send')
var statusMsg = document.getElementById('status')
var app = document.getElementById('app')
var testElem = document.getElementById('test');

var questionIndex = 0

var data = {
  name: 'cool dude',
  questionType: 'text',
  answers: [
    'nice',
    'butts',
    'rad'
  ]
}

var ipAddressRegex = /(\d){1,3}.(\d){1,3}.(\d){1,3}.(\d){1,3}/

function init() {
  button.addEventListener('click', sendMessage, false)
}
init()


var serverConnect = function () {
  var element = document.getElementById('server-connect')
  var serverInput = document.getElementById('server-input')
  var nameInput = document.getElementById('name-input')
  var connectButton = document.getElementById('connect-button')

  function toggleElement(force) {
    element.classList.toggle('hidden', !force)
  }

  function clearIpInput() {
    serverInput.value = ''
  }

  function init() {
    serverInput.addEventListener('keydown', connectServer, false)
    connectButton.addEventListener('click', connectServer, false)
    console.log('server init')
  }
  init()

  function connectServer(e) {
    if (e.keyCode === 13 || e.keyCode === undefined) {
      var server = serverInput.value
      if (server.match(ipAddressRegex)) {
        setStatus('trying to connect')
        sock = new WebSocket('ws://' + server + ':9999')
        sock.addEventListener('open', connect, false)
        sock.addEventListener('message', onMessage, false)
        sock.addEventListener('error', onError, false)
        sock.addEventListener('close', onClose, false)
      } else {
        setStatus('incorrect format for ip address!')
      }
    }
  }

  return {
    toggleElement: toggleElement,
    clearIpInput: clearIpInput
  }
}()

function setStatus(text) {
  console.log(text)
  statusMsg.textContent = text
}

function connect() {
  serverConnect.toggleElement(false)

  _data = JSON.stringify(data)
  sock.send(_data)
  console.log('sent ↓')
  console.log(_data)
}

function onMessage(e) {
  message.textContent = e.data
  console.log('got ' + e.data)
}

function sendMessage() {
  data.answers[questionIndex] = input.value
  sock.send(data)
}

function onError(e) {
  setStatus('there was an issue connecting with this server,\ndouble check your connection and ip address');
}

function onClose() {
  setStatus('connection closed')
  serverConnect.toggleElement(true)
  serverConnect.clearIpInput()
}


