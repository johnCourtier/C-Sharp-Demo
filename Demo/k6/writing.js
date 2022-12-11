import { check } from 'k6';
import exec from 'k6/execution';
import http from 'k6/http';

export let options = {
    vus: 10,
    duration: '5s'
};

export default function () {
    const res = http.post('http://host.docker.internal:8080/documents/', JSON.stringify({
        id: String(exec.scenario.iterationInInstance),
        tags: ["demo", "k6"],
        data: {
            title: "k6 test",
            data: "k6 data"
        }
    }), {
        headers: { 'Content-Type': 'application/json' },
    });
    check(res, {
        'is status 201': (r) => r.status === 201,
    });
}