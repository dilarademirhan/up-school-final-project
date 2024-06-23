import { useContext, useState } from "react";
import { getClaimsFromJwt } from "../utils/jwtHelper";
import { toast } from "react-toastify";
import { useNavigate, Link as RouterLink } from "react-router-dom";
import axios from 'axios';
import { Box, Button, Container, Grid, TextField, Link } from '@mui/material';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import { AppUserContext } from "../context/StateContext";
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';

const BASE_URL = import.meta.env.VITE_API_URL;
const theme = createTheme();

function Login() {
    const { setAppUser } = useContext(AppUserContext);
    const navigate = useNavigate();
    const [authLoginCommand, setAuthLoginCommand] = useState({ email: "", password: "" });

    const handleSubmit = async (event) => {
        event.preventDefault();

        try {
            const response = await axios.post(`${BASE_URL}/Authentication/Login`, authLoginCommand, {
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            if (response.status === 200) {
                const accessToken = response.data.accessToken;
                const { uid, email, given_name, family_name } = getClaimsFromJwt(accessToken);
                const expires = response.data.expires;

                setAppUser({ id: uid, email, firstName: given_name, lastName: family_name, expires, accessToken });

                const localJwt = {
                    accessToken,
                    expires
                }

                localStorage.setItem("user_jwt", JSON.stringify(localJwt));
                toast.success("You have successfully logged in!");
                navigate("/");
            } else {
                console.log("else", response);
            }
        } catch (error) {
            console.log("catch");
            console.log(error);

            toast.error("An error occured!");
        }
    }

    const handleInputChange = (event) => {
        setAuthLoginCommand({
            ...authLoginCommand,
            [event.target.name]: event.target.value
        });
    }

    const onGoogleLoginClick = (e) => {
        e.preventDefault();
        window.location.href = `${BASE_URL}/Authentication/GoogleSignInStart`;
    };

    return (
        <ThemeProvider theme={theme}>
            <Container component="main" maxWidth="xs">
            <LockOutlinedIcon
                fontSize="medium"
                sx={{
                    m: 1,
                    bgcolor: 'primary.main',
                    color: 'white',
                    borderRadius: '50%',
                    padding: '0.5rem'
                }}
            />
            <div
                style={{
                    margin: 0,
                    marginBottom: '1rem',
                    fontFamily: "Roboto, Helvetica, Arial, sans-serif",
                    fontWeight: 400,
                    fontSize: "1.5rem",
                    lineHeight: 1.334,
                    letterSpacing: "0em"
                }}
            >
                Login
            </div>
                <form onSubmit={handleSubmit}>
                    <Grid container spacing={2} justifyContent="center">
                        <Grid item xs={12}>
                            <TextField
                                fullWidth
                                id="email"
                                name="email"
                                label="Email"
                                variant="outlined"
                                value={authLoginCommand.email}
                                onChange={handleInputChange}
                                InputProps={{
                                    endAdornment: (
                                        <Box sx={{ color: 'action.active', marginRight: 1 }}>
                                            <i className="fas fa-envelope"></i>
                                        </Box>
                                    ),
                                }}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <TextField
                                fullWidth
                                id="password"
                                name="password"
                                label="Password"
                                type="password"
                                variant="outlined"
                                value={authLoginCommand.password}
                                onChange={handleInputChange}
                                InputProps={{
                                    endAdornment: (
                                        <Box sx={{ color: 'action.active', marginRight: 1 }}>
                                            <i className="fas fa-lock"></i>
                                        </Box>
                                    ),
                                }}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <Button
                                type="submit"
                                fullWidth
                                variant="contained"
                                sx={{ mt: 3, mb: 2 }}
                            >
                                Login
                            </Button>
                            <Button
                                fullWidth
                                variant="contained"
                                sx={{ bgcolor: 'red' }}
                                onClick={onGoogleLoginClick}
                            >
                                <i className="fab fa-google mr-2"></i> Continue with Google
                            </Button>
                        </Grid>
                    </Grid>
                </form>
                <Grid container justifyContent="flex-end" sx={{ marginTop: 2 }}>
                    <Grid item>
                        <Link component={RouterLink} to="/sign-up" variant="body2" sx={{ display: 'flex', alignItems: 'center' }}>
                            Dont have an account? Sign Up
                        </Link>
                    </Grid>
                </Grid>
            </Container>
        </ThemeProvider>
    );
}

export default Login;
