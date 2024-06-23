import { useEffect } from "react";
import './App.css';
import OrderForm from './components/OrderForm';
import OrdersTable from './components/OrdersTable';
import CrawlerLiveLogs from './components/CrawlerLiveLogs';
import NavBar from './components/NavBar';
import SignUp from "./components/SignUp";
import SocialLogin from "./components/SocialLogin";
import { useState } from 'react';
import { ToastContainer } from 'react-toastify';
import {  Route, Routes } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import Login from './components/Login';
import 'react-toastify/dist/ReactToastify.css';
import { getClaimsFromJwt } from './utils/jwtHelper';
import { AppUserContext } from "./context/StateContext";



function App() {
  const navigate = useNavigate();

  const [appUser, setAppUser] = useState(undefined);

  useEffect(() => {
    const jwtJson = localStorage.getItem("user_jwt");

    if (!jwtJson) {
      navigate("/login");
      return;
    }

    const localJwt = JSON.parse(jwtJson);

    const { uid, email, given_name, family_name } = getClaimsFromJwt(localJwt.accessToken);
    const expires = localJwt.expires;

    setAppUser({
      id: uid,
      email: email,
      firstName: given_name,
      lastName: family_name,
      expires,
      accessToken: localJwt.accessToken
    });
  }, []);

  return (
      <>
        <AppUserContext.Provider value={{ appUser, setAppUser }}>
          <ToastContainer/>
          
            <NavBar />
            <Routes>
              <Route path="/" element={<OrderForm/>}/>
              <Route path="/login" element={<Login/>}/>
              <Route path="/sign-up" element={<SignUp />} />  
              <Route path="/live-logs" element={<CrawlerLiveLogs/>}/>
              <Route path="/social-login" element={<SocialLogin/>}/>
              <Route path="/add-order" element={<OrderForm/>}/>
              <Route path="/orders" element={<OrdersTable/>}/>
            </Routes>
        </AppUserContext.Provider>
      </>
  )
}

export default App;
