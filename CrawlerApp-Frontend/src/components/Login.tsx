import React from "react";
import {Button, Icon} from "semantic-ui-react";

const BASE_URL = import.meta.env.VITE_API_URL;

function Login() {
    const onGoogleLoginClick = (e:React.FormEvent) => {
        e.preventDefault();
        window.location.href = `${BASE_URL}/GoogleLoginController/GoogleSignInStart`;
    };

    return (
        <>
        <h2>Login Page</h2>
        <Button fluid onClick={onGoogleLoginClick} type="button">
            <Icon name='google' /> Sign in with Google
        </Button>
    </>
    );
}

export default Login;