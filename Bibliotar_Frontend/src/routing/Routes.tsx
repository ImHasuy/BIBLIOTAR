import Login from "../pages/Login.tsx";
import Dashboard from "../pages/Dashboard.tsx";
import Landing from "../pages/Landing.tsx";
import Register from "../pages/Register.tsx";
import BookDetails from "../pages/BookDetails.tsx";
import Profile from "../pages/Profile.tsx";
import Books from "../pages/Books.tsx";
import Reservations from "../pages/Reservations.tsx";
import Borrows from "../pages/Borrows.tsx";



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
    {
        path: "profile",
        component: <Profile/>,
        isPrivate: true
    },
    {
        path: "books",
        component: <Books/>,
        isPrivate: true
    },
    {
        path: "Reservations",
        component: <Reservations/>,
        isPrivate: true
    },
    {
        path: "Borrows",
        component: <Borrows/>,
        isPrivate: true
    },

];