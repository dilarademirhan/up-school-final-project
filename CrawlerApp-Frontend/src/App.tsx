import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import './App.css';
import Login from './components/Login';
import Settings from './components/Settings';
import Order from './components/Order';

function App() {
  return (
    <>
      <Router>
        <nav>
          <ul>
            <li>
              <Link to="/">Login</Link>
            </li>
            <li>
              <Link to="/order">Order</Link>
            </li>
            <li>
              <Link to="/settings">Settings</Link>
            </li>
          </ul>
        </nav>

        <Routes>
          <Route path="/" element={<Login />} />
          <Route path="/order" element={<Order />} />
          <Route path="/settings" element={<Settings />} />
        </Routes>
      </Router>
      </>
  );
}

export default App;
