import {
    Stack,
    TextInput,
    PasswordInput,
    Group,
    Button,
    Anchor,
    Divider,
    Alert,
    LoadingOverlay
} from "@mantine/core";
import { IconAlertCircle } from "@tabler/icons-react";
import { useForm } from "@mantine/form";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import AuthContainer from "../components/AuthContainer.tsx";
import api from "../api/api";

const Register = () => {
    const navigate = useNavigate();
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const form = useForm({
        initialValues: {
            email: '',
            name: '',
            password: '',
            confirmPassword: '',
            phoneNumber: '',
            zipCode: '',
            city: '',
            street: '',
            houseNumber: '',
            country: ''
        },

        validate: {
            email: (val: string) => (/^\S+@\S+$/.test(val) ? null : 'Érvénytelen e-mail cím'),
            name: (val: string) => (val.length < 2 ? 'A név túl rövid' : null),
            password: (val: string) => (val.length <= 6 ? 'A jelszónak legalább 6 karakter hosszúnak kell lennie.' : null),
            confirmPassword: (val: string, values) =>
                val !== values.password ? 'A jelszavak nem egyeznek' : null,
            phoneNumber: (val: string) => (val.length < 6 ? 'Érvénytelen telefonszám' : null),
            zipCode: (val: string) => (val.length < 2 ? 'Érvénytelen irányítószám' : null),
            city: (val: string) => (val.length < 2 ? 'Érvénytelen város' : null),
            street: (val: string) => (val.length < 2 ? 'Érvénytelen utca' : null),
            houseNumber: (val: string) => (val.length < 1 ? 'Érvénytelen házszám' : null),
            country: (val: string) => (val.length < 2 ? 'Érvénytelen ország' : null),
        },
    });

    const handleSubmit = async (values: typeof form.values) => {
        setIsLoading(true);
        setError(null);
        
        try {
            const userData = {
                email: values.email,
                phoneNumber: values.phoneNumber,
                name: values.name,
                password: values.password,
                zipCode: values.zipCode,
                city: values.city,
                street: values.street,
                houseNumber: values.houseNumber,
                country: values.country
            };
            await api.User.createUser(userData);
            navigate('/login');
        } catch (err) {
            console.error('Registration error:', err);
            setError('A regisztráció sikertelen. Kérjük, próbálja újra később.');
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <AuthContainer>
            <div style={{ position: 'relative' }}>
                <LoadingOverlay visible={isLoading} overlayProps={{ blur: 2 }} />
                
                {error && (
                    <Alert icon={<IconAlertCircle size="1rem" />} title="Hiba!" color="red" mb="md">
                        {error}
                    </Alert>
                )}
                
                <form onSubmit={form.onSubmit(handleSubmit)}>
                    <Stack>
                        <TextInput
                            required
                            label="Teljes név"
                            placeholder="Kovács János"
                            key={form.key('name')}
                            radius="md"
                            {...form.getInputProps('name')}
                        />

                        <TextInput
                            required
                            label="E-mail cím"
                            placeholder="pelda@email.hu"
                            key={form.key('email')}
                            radius="md"
                            {...form.getInputProps('email')}
                        />
                        
                        <TextInput
                            required
                            label="Telefonszám"
                            placeholder="+36201234567"
                            key={form.key('phoneNumber')}
                            radius="md"
                            {...form.getInputProps('phoneNumber')}
                        />

                        <Group grow>
                            <TextInput
                                required
                                label="Irányítószám"
                                placeholder="1234"
                                key={form.key('zipCode')}
                                radius="md"
                                {...form.getInputProps('zipCode')}
                            />
                            
                            <TextInput
                                required
                                label="Város"
                                placeholder="Budapest"
                                key={form.key('city')}
                                radius="md"
                                {...form.getInputProps('city')}
                            />
                        </Group>
                        
                        <Group grow>
                            <TextInput
                                required
                                label="Utca"
                                placeholder="Példa utca"
                                key={form.key('street')}
                                radius="md"
                                {...form.getInputProps('street')}
                            />
                            
                            <TextInput
                                required
                                label="Házszám"
                                placeholder="123"
                                key={form.key('houseNumber')}
                                radius="md"
                                {...form.getInputProps('houseNumber')}
                            />
                        </Group>
                        
                        <TextInput
                            required
                            label="Ország"
                            placeholder="Magyarország"
                            key={form.key('country')}
                            radius="md"
                            {...form.getInputProps('country')}
                        />

                        <PasswordInput
                            required
                            label="Jelszó"
                            placeholder="Jelszavad"
                            key={form.key('password')}
                            radius="md"
                            {...form.getInputProps('password')}
                        />

                        <PasswordInput
                            required
                            label="Jelszó megerősítése"
                            placeholder="Jelszó ismét"
                            key={form.key('confirmPassword')}
                            radius="md"
                            {...form.getInputProps('confirmPassword')}
                        />
                    </Stack>

                    <Group justify="space-between" mt="xl">
                        <Anchor component="button" type="button" c="dimmed" onClick={() => navigate('/login')} size="xs">
                            Már van fiókod? Jelentkezz be
                        </Anchor>
                        <Button type="submit" radius="xl">
                            Regisztráció
                        </Button>
                    </Group>
                    <Divider my="lg"/>
                </form>
            </div>
        </AuthContainer>
    );
};

export default Register;