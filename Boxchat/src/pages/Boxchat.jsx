import React from 'react'

export default function Boxchat({ userId }) {
    const [messages, setMessages] = useState([]);
    const [input, setInput] = useState("");
    const ws = React.useRef(null);

    useEffect(() => {
        ws.current = new WebSocket(`ws://localhost:7135/chat?userId=${userId}`);
        ws.current.onmessage = (event) => {
            setMessages((prev) => [...prev, event.data]);
        };
        return () => ws.current.close();
    }, [userId]);

    const sendMessage = () => {
        const message = JSON.stringify({ targetUser: "targetUserId", message: input });
        ws.current.send(message);
        setMessages((prev) => [...prev, `You: ${input}`]);
        setInput("");
    };
    return (
        <div>
      <div style={{ border: "1px solid black", height: "300px", overflowY: "scroll" }}>
        {messages.map((msg, index) => (
          <div key={index}>{msg}</div>
        ))}
      </div>
      <input
        type="text"
        value={input}
        onChange={(e) => setInput(e.target.value)}
        placeholder="Type a message..."
      />
      <button onClick={sendMessage}>Send</button>
    </div>
    )
}
