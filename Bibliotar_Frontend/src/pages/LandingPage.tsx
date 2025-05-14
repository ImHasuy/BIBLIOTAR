import { useNavigate } from 'react-router-dom';
import { Button, Container, Group, Stack, Text, Title, rem } from '@mantine/core';
import { IconBooks, IconHome, IconList, IconUsers } from '@tabler/icons-react';
import classes from '../components/navbar/NavbarMinimalColored.module.css';

interface FeatureCardProps {
    icon: typeof IconHome;
    title: string;
    description: string;
    color: string;
}

function FeatureCard({ icon: Icon, title, description, color }: FeatureCardProps) {
    return (
        <div className={classes.link} style={{ cursor: 'default' }}>
            <Button variant="light" color={color} className={classes.iconButton}
                    style={{ width: rem(40), height: rem(40), flexGrow: 0, flexShrink: 0, flexBasis: rem(40) }}>
                <Icon className={classes.linkIcon} style={{ width: rem(25), height: rem(25), flexGrow: 0, flexShrink: 0, flexBasis: rem(25) }} stroke={1.8} />
            </Button>
            <div>
                <Text fw={500}>{title}</Text>
                <Text size="sm" c="dimmed">{description}</Text>
            </div>
        </div>
    );
}

export function LandingPage() {
    const navigate = useNavigate();

    const features = [
        {
            icon: IconBooks,
            title: 'Könyvtár kezelés',
            description: 'Fedezze fel könyvgyűjteményünket és kezelje kölcsönzéseit',
            color: 'app-color'
        },
        {
            icon: IconUsers,
            title: 'Felhasználói felület',
            description: 'Személyre szabott felhasználói élmény minden olvasó számára',
            color: 'app-color'
        },
        {
            icon: IconList,
            title: 'Kölcsönzések követése',
            description: 'Egyszerűen követheti kölcsönzéseit és határidőket',
            color: 'app-color'
        }
    ];

    return (
        <Container size="md" py="xl">
            <Stack align="center" spacing="xl">
                <Title order={1} ta="center">Üdvözöljük a BiblioTár rendszerben!</Title>
                <Text size="lg" ta="center" c="dimmed">
                    A modern könyvtárkezelő rendszer, amely egyszerűvé teszi a könyvek kezelését és kölcsönzését.
                </Text>

                <Stack spacing="md" mt="md" w="100%">
                    {features.map((feature, index) => (
                        <FeatureCard key={index} {...feature} />
                    ))}
                </Stack>

                <Group mt="xl">
                    <Button size="lg" variant="filled" onClick={() => navigate('/login')}>
                        Bejelentkezés
                    </Button>
                    <Button size="lg" variant="outline" onClick={() => navigate('/register')}>
                        Regisztráció
                    </Button>
                </Group>
            </Stack>
        </Container>
    );
}
