import React, { useEffect, useState } from 'react';
import * as signalR from "@microsoft/signalr";
import { Box, Paper, List, ListItem, ListItemText, Divider } from '@mui/material';

const CrawlerLiveLogs = () => {
    const [logs, setLogs] = useState([]);
    const [hubConnection, setHubConnection] = useState(null);

    useEffect(() => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:7010/Hubs/SeleniumLogHub")
            .withAutomaticReconnect()
            .build();

        connection.start().then(() => {
            console.log("Connected to LogHub");
            connection.on("NewSeleniumLogAdded", log => {
                console.log("Received new log:", log);
                setLogs(prevLogs => [...prevLogs, log]);
            });
        }).catch(err => console.error("Connection error: ", err));

        setHubConnection(connection);

        return () => {
            connection.stop().then(() => {
                console.log("Disconnected from LogHub");
            }).catch(err => console.error("Disconnection error: ", err));
        };
    }, []);

    return (
        <Box sx={{ mt: 5, p: 3 }}>
            <Paper elevation={3} sx={{ maxHeight: 400, overflow: 'auto' }}>
                <List>
                    {logs.map((log, index) => (
                        <React.Fragment key={index}>
                            <ListItem>
                                <ListItemText
                                    primary={log.message}
                                    secondary={new Date(log.sentOn).toLocaleString()}                                    
                                />
                            </ListItem>
                            {index < logs.length - 1 && <Divider />}
                        </React.Fragment>
                    ))}
                </List>
            </Paper>
        </Box>
    );
};

export default CrawlerLiveLogs;
