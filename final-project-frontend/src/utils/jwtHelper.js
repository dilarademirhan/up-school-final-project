import { jwtDecode } from "jwt-decode";

export function getClaimsFromJwt (token) {

    const decodedJwt = jwtDecode(token);

    if (!decodedJwt || typeof decodedJwt !== "object")
        return {};

    const {uid, email, given_name, family_name, jti} = decodedJwt;

    return {
        uid,
        email,
        given_name,
        family_name,
        jti
    }
}

