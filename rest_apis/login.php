<?php

    include 'db_conn.php';

    $db_conn = new DBobject;
    $username = $_GET['username'];
    $password = $_GET['password'];

    $response = $db_conn->validate_user($username, $password);

    echo $response;

    die();

?>