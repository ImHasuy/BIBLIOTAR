import Login from "../pages/Login.tsx";
import Dashboard from "../pages/Dashboard.tsx";
import Landing from "../pages/Landing.tsx";
import Register from "../pages/Register.tsx";
import BookDetails from "../pages/BookDetails.tsx";
import Profile from "../pages/Profile.tsx";
import Books from "../pages/Books.tsx";
import Reservations from "../pages/Reservations.tsx";
import Borrows from "../pages/Borrows.tsx";
import BorrowManager from "../pages/BorrowManager.tsx";
import BooksManager from "../pages/BooksManager";
import BookCreate from "../pages/BookCreate";
import BookEdit from "../pages/BookEdit";
import BorrowCreate from "../pages/BorrowCreate.tsx";
import BorrowForm from "../pages/BorrowForm.tsx";






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
    {
        path: "BorrowManager",
        component: <BorrowManager/>,
        isPrivate: true
    },
    {
        path: "BooksManager",
        component: <BooksManager />,
        isPrivate: true,
    },
    {
        path: "BookCreate",
        component: <BookCreate />,
        isPrivate: true,
    },
    {
        path: "BookEdit/:id",
        component: <BookEdit />,
        isPrivate: true,
    },
    {
        path: "BorrowCreate",
        component: <BorrowCreate />,
        isPrivate: true,
    },
    {
        path: "BorrowForm/:bookId",
        component: <BorrowForm />,
        isPrivate: true,
    }
];