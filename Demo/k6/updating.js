import { check } from 'k6';
import http from 'k6/http';

export let options = {
    vus: 10,
    duration: '5s'
};

export function setup() {
    http.post('http://host.docker.internal:8080/documents/', JSON.stringify({
        id: "id123",
        tags: ["demo", "k6"],
        data: {
            title: "k6 test",
            data: "k6 data"
        }
    }), {
        headers: { 'Content-Type': 'application/json' },
    });
}

export default function () {
    const res = http.put('http://host.docker.internal:8080/documents/', JSON.stringify({
        id: "id123",
        tags: ["demo", "k6", "updated"],
        data: {
            title: "k6 test",
            data: "k6 data"
        }
    }), {
        headers: { 'Content-Type': 'application/json' },
    });
    check(res, {
        'is status 204': (r) => r.status === 204,
    });
}