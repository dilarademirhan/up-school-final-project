import { useSearchParams, useNavigate } from "react-router-dom";
import { useContext, useEffect } from "react";
import { getClaimsFromJwt } from "../utils/jwtHelper";import { AppUserContext } from "../context/StateContext";

function SocialLogin() {
    const [searchParams] = useSearchParams(); 
    const navigate = useNavigate();
    const { setAppUser } = useContext(AppUserContext);

    useEffect(() => {
        const accessToken = searchParams.get("access_token");
        const expiryDate = searchParams.get("expiry_date");

        if (accessToken && expiryDate) {
            const { uid, email, given_name, family_name } = getClaimsFromJwt(accessToken);
            console.log(uid, email, given_name, family_name);
            const expires = expiryDate;

            setAppUser({ id: uid, email, firstName: given_name, lastName: family_name, expires, accessToken });

            const localJwt = {
                accessToken,
                expires
            };

            localStorage.setItem("user_jwt", JSON.stringify(localJwt));
            navigate("/");
        }
    }, []);

    return (
        <div></div>
    );
}

export default SocialLogin;
