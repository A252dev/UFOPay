const dayLabels = [
    '12AM',
    '1AM',
    '2AM',
    '3AM',
    '4AM',
    '5AM',
    '6AM',
    '7AM',
    '8AM',
    '9AM',
    '10AM',
    '11AM',
    '12PM',
    '1PM',
    '2PM',
    '3PM',
    '4PM',
    '5PM',
    '6PM',
    '7PM',
    '8PM',
    '9PM',
    '10PM',
    '11PM',
];

const weekLabels = [
    'MONDAY',
    'TUESDAY',
    'WEDNESDAY',
    'THURSDAY',
    'FRIDAY',
    'SATURDAY',
    'SUNDAY',
];

const monthLabels = [
    '1DAY',
    '2DAY',
    '3DAY',
    '4DAY',
    '5DAY',
    '6DAY',
    '7DAY',
    '8DAY',
    '9DAY',
    '10DAY',
    '11DAY',
    '12DAY',
    '13DAY',
    '14DAY',
    '15DAY',
    '16DAY',
    '17DAY',
    '18DAY',
    '19DAY',
    '20DAY',
    '21DAY',
    '22DAY',
    '23DAY',
    '24DAY',
    '25DAY',
    '26DAY',
    '27DAY',
    '28DAY',
    '29DAY',
    '30DAY',
];

const yearLabels = [
    'JAN',
    'FEB',
    'MAR',
    'APR',
    'MAY',
    'JUN',
    'JUL',
    'AUG',
    'SEP',
    'OCT',
    'NOV',
    'DEC',
];


const dayData = {
    labels: dayLabels,
    datasets: [{
        label: 'All orders',
        backgroundColor: '#7984e4',
        borderColor: '#3a4181',
        data: [4, 2, 7, 2, 1, 5, 8, 12, 6, 2, 1, 7, 1, 18, 22, 52, 15, 83, 14, 52, 12, 74, 21, 51]
    },
    {
        label: 'Paid orders',
        backgroundColor: '#000f9c',
        borderColor: '#00109c',
        data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
    }]
};


const weekData = {
    labels: weekLabels,
    datasets: [{
        label: 'All orders',
        backgroundColor: '#7984e4',
        borderColor: '#3a4181',
        data: [4, 2, 7, 2, 1, 5, 8]
    },
    {
        label: 'Paid orders',
        backgroundColor: '#000f9c',
        borderColor: '#00109c',
        data: [0, 0, 0, 0, 0, 0, 0]
    }]
};

const monthData = {
    labels: monthLabels,
    datasets: [{
        label: 'All orders',
        backgroundColor: '#7984e4',
        borderColor: '#3a4181',
        data: [4, 2, 3, 23, 45, 234, 435, 234, 234, 1, 54, 234, 52, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
    },
    {
        label: 'Paid orders',
        backgroundColor: '#000f9c',
        borderColor: '#00109c',
        data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
    }]
};

const yearData = {
    labels: yearLabels,
    datasets: [{
        label: 'All orders',
        backgroundColor: '#7984e4',
        borderColor: '#3a4181',
        data: [4, 2, 7, 2, 1, 5, 8, 4, 2, 7, 2, 1]
    },
    {
        label: 'Paid orders',
        backgroundColor: '#000f9c',
        borderColor: '#00109c',
        data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
    }]
};


const dayConfig = {
    type: 'line',
    data: dayData,
    options: {}
};

const weekConfig = {
    type: 'line',
    data: weekData,
    options: {}
};

const monthConfig = {
    type: 'line',
    data: monthData,
    options: {}
};

const yearConfig = {
    type: 'line',
    data: yearData,
    options: {}
};

const dayChartButton = document.getElementById('day_btw');
const weekChartButton = document.getElementById('week_btw');
const monthChartButton = document.getElementById('month_btw');
const yearChartButton = document.getElementById('year_btw');

const dayChartContent = document.getElementById('dayChartContent');
const weekChartContent = document.getElementById('weekChartContent');
const monthChartContent = document.getElementById('monthChartContent');
const yearChartContent = document.getElementById('yearChartContent');

const dayChart = new Chart(
    document.getElementById('dayChart'),
    dayConfig
);

dayChartButton.addEventListener('click', e => {
    dayChartContent.classList.remove('hidden');

    weekChartContent.classList.add('hidden');
    monthChartContent.classList.add('hidden');
    yearChartContent.classList.add('hidden');

});

weekChartButton.addEventListener('click', e => {
    weekChartContent.classList.remove('hidden');

    dayChartContent.classList.add('hidden');
    monthChartContent.classList.add('hidden');
    yearChartContent.classList.add('hidden');

    const weekChart = new Chart(
        document.getElementById('weekChart'),
        weekConfig
    );
});

monthChartButton.addEventListener('click', e => {
    monthChartContent.classList.remove('hidden');

    dayChartContent.classList.add('hidden');
    weekChartContent.classList.add('hidden');
    yearChartContent.classList.add('hidden');

    const monthChart = new Chart(
        document.getElementById('monthChart'),
        monthConfig
    );
});

yearChartButton.addEventListener('click', e => {
    yearChartContent.classList.remove('hidden');

    weekChartContent.classList.add('hidden');
    dayChartContent.classList.add('hidden');
    monthChartContent.classList.add('hidden');

    const yearChart = new Chart(
        document.getElementById('yearChart'),
        yearConfig
    );
});
