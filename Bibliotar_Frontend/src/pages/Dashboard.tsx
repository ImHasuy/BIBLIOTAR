import { Container, Title, Text, Box, Center } from '@mantine/core';

function Dashboard() {
  return (
    <Container>
      <Center style={{ height: '100%' }}>
        <Box my="lg" style={{ textAlign: 'center' }}>
          <Title order={1} mb="md">
            Üdvözöljük a Bibliotar rendszerben!
          </Title>
          <Text size="lg">
            Válasszon az oldalsávon található menüpontok közül!
          </Text>
        </Box>
      </Center>
    </Container>
  );
}

export default Dashboard;