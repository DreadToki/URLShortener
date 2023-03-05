export const BaseUrl = process.env.REACT_APP_API_URL;

// Urls
export const ReadUrls = `${BaseUrl}url/read`;
export const ReadUrl = `${BaseUrl}url`;
export const DeleteUrl = `${BaseUrl}url/delete`;
export const CreateUrl = `${BaseUrl}url/create`;

// User
export const RegisterUser = `${BaseUrl}auth/register`;
export const LoginUser = `${BaseUrl}auth/login`;
export const ReadUser = `${BaseUrl}auth/user`;
export const IsElevatedUser = `${BaseUrl}auth/iselevateduser`;
export const LogoutUser = `${BaseUrl}auth/logout`;

// About
export const ReadAbout = `${BaseUrl}about/read`;
export const PostAbout = `${BaseUrl}about/create`;
