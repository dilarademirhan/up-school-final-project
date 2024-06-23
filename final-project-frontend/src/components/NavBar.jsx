import { useContext } from 'react';
import { Link as RouterLink, useNavigate } from 'react-router-dom';
import { AppBar, Toolbar, IconButton, Tooltip, Box } from '@mui/material';
import { AppUserContext } from '../context/StateContext';
import AddBoxIcon from '@mui/icons-material/AddBox';
import LogoutIcon from '@mui/icons-material/Logout';
import OrdersIcon from '@mui/icons-material/Receipt';
import logo from '/logo.png'

const NavBar = () => {
    const { appUser, setAppUser } = useContext(AppUserContext);
    const navigate = useNavigate();

    const handleLogout = () => {
        localStorage.removeItem('user_jwt');
        setAppUser(undefined);
        navigate(0);
    };

    return (
        <AppBar position="fixed">
            <Toolbar>
                <RouterLink to="/" style={{ flexGrow: 1, textDecoration: 'none', display: 'flex', alignItems: 'center' }}>
                    <img src={logo} alt="Logo" style={{ height: '40px', marginRight: '10px' }} />
                </RouterLink>
                <Box>
                    {appUser ? (
                        <>
                            <Tooltip title="Add Order">
                                <IconButton sx={{ color: 'white' }} component={RouterLink} to="/add-order">
                                    <AddBoxIcon />
                                </IconButton>
                            </Tooltip>
                            <Tooltip title="Orders">
                                <IconButton sx={{ color: 'white' }} component={RouterLink} to="/orders">
                                    <OrdersIcon />
                                </IconButton>
                            </Tooltip>
                            <Tooltip title="Logout">
                                <IconButton sx={{ color: 'white' }} onClick={handleLogout}>
                                    <LogoutIcon />
                                </IconButton>
                            </Tooltip>
                        </>
                    ) : (
                        <></>
                    )}
                </Box>
            </Toolbar>
        </AppBar>
    );
};

export default NavBar;
