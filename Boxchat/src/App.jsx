import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import users from './data/users'
import Boxchat from './pages/Boxchat'

function App() {
  const [userId, setUserId] = useState()
  const [isLog, setLog] = useState(false)
  const [user, setUser] = useState()
  const Login = () => {
    const user = users.find(u => u.userId === userId)
    if (user) {
      setLog(true)
      setUser(user)
    }
  }
  return (
    <>
      {!isLog &&
        <div>
          UserId: <input value={userId} onChange={e => setUserId(e.target.value)} />
          <button onClick={Login}>Login</button>
        </div>}

      {isLog &&
        <Boxchat />}
    </>
  )
}

export default App
