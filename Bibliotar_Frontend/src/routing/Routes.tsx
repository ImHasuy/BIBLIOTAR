import Login from "../pages/Login.tsx";
import Dashboard from "../pages/Dashboard.tsx";
import Landing from "../pages/Landing.tsx";
import Register from "../pages/Register.tsx";
import BookDetails from "../pages/BookDetails.tsx";

export const routes = [
    {
        path: "landing",
        component: <Landing/>,
        isPrivate: false
    },
    {
        path: "login",
        component: <Login/>,
        isPrivate: false
    },
    {
        path: "register",
        component: <Register/>,
        isPrivate: false
    },
    {
        path: "books/:id",
        component: <BookDetails/>,
        isPrivate: false
    },
    {
        path: "dashboard",
        component: <Dashboard/>,
        isPrivate: true
    },
];