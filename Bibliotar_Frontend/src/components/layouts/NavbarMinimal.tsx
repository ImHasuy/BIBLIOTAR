import {useContext, useEffect, useState} from "react";
import {rem, Button, useMantineTheme} from "@mantine/core";
import {
    IconUserCircle,
    IconLogout,
    IconHome, IconList, IconUsers, IconBooks, IconAsset, IconAd, IconAutomation
} from "@tabler/icons-react";
import classes from "./NavbarMinimalColored.module.css";
import {useNavigate} from "react-router-dom";
import {useMediaQuery} from "@mantine/hooks";
import useAuth from "../../hooks/useAuth.tsx";
import {AuthContext} from "../../context/AuthContext.tsx";

interface NavbarLinkProps {
    icon: typeof IconHome;
    label: string;
    color: string;
    active?: boolean;

    onClick?(): void;
}

function NavbarLink({icon: Icon, label, color, active, onClick}: NavbarLinkProps) {

    return (
        <div
            role="button"
            className={classes.link} color={color}
            onClick={onClick}
            data-active={active || undefined}
        >
            <Button variant="light" color={color} className={classes.iconButton} style={{width: rem(40), height: rem(40), flexGrow: 0, flexShrink:0, flexBasis: rem(40)}}>
                <Icon className={classes.linkIcon} style={{width: rem(25), height: rem(25), flexGrow: 0, flexShrink:0, flexBasis: rem(25)}} stroke={1.8}/>
            </Button>
            <span>{label}</span>
        </div>
    );
}

export function NavbarMinimal({toggle}: any) {
    const theme = useMantineTheme();
    const isMobile = useMediaQuery(`(max-width: ${theme.breakpoints.sm})`);
    const [active, setActive] = useState(0);
    const navigate = useNavigate();
    const {logout} = useAuth();
    const {role} = useContext(AuthContext);

    const menuItems = [
        {
            icon: IconHome,
            label: "Kezdőlap",
            url: "dashboard",
            roles: ['Administrator', 'Librarian', 'Customer']
        },
        {
            icon: IconBooks,
            label: "Könyvek",
            url: "Books",
            roles: ['Customer']
        },
        {
            icon: IconList,
            label: "Foglalásaim",
            url: "Reservations",
            roles: ['Customer']
        },
        {
            icon: IconAsset,
            label: "Kölcsönzéseim",
            url: "Borrows",
            roles: ['Customer']
        },
        {
            icon: IconAutomation,
            label: "Kölcsönzések létrehozása",
            url: "BorrowCreate",
            roles: ['Librarian']
        },
        {
            icon: IconAsset,
            label: "Kölcsönzések kezelése",
            url: "BorrowManager",
            roles: ['Librarian']
        },
        {
            icon: IconList,
            label: "Könyvek kezelése",
            url: "BooksManager",
            roles: ['Administrator']
        }
    ];


    const onLogout = () => {
        logout();
    }

    useEffect(() => {
        setActive(menuItems.findIndex(m => location.pathname === m.url));
    }, [])

    const links = menuItems.filter(item => item.roles.includes(role ?? ''))
        .map((link, index) => (
            <NavbarLink
                color="app-color"
                {...link}
                key={link.label}
                active={index === active}
                onClick={async () => {
                    setActive(index);
                    toggle();
                    navigate(link.url);
                }}
            />
        ));

    return (
        <nav className={classes.navbar}>
            <div>
                <div className={classes.navbarMain}>
                    {links}
                </div>
                <div className={classes.footer} style={{width: !isMobile ? '216px' : '90%'}}>
                    <NavbarLink
                        active={location.pathname === '/profile'}
                        icon={IconUserCircle}
                        label="Profil"
                        onClick={() => {
                            navigate("profile");
                            toggle();
                        }} color="grape" />
                    <NavbarLink
                        icon={IconLogout}
                        label={"Kijelentkezés"}
                        onClick={onLogout} color="grape"/>

                </div>
            </div>
        </nav>
    );
}
