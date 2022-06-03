<?php
    include 'db_conn.php';

    $db_conn = new DBobject;

    $name = $_GET['name'];

    $response = $db_conn->search_data($name);

    echo $response;

    die();

?>