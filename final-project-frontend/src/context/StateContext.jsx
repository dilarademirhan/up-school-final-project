import { createContext } from 'react';

export const AppUserContext = createContext({
    appUser: undefined,
    setAppUser: () => {},
});
