﻿using System;
using System.Threading;
using Moq;
using NUnit.Framework;
//using RestSharp;
//using RestSharp.Deserializers;
using Simple;

using Twilio.TaskRouter;
using Twilio.Api.Tests.Integration;
using Twilio.Api.Tests;

namespace Twilio.TaskRouter.Tests
{
    [TestFixture]
    public class ReservationTests
    {
        private const string RESERVATION_SID = "WR123";

        private const string TASK_SID = "WT123";

        private const string WORKSPACE_SID = "WS123";

        ManualResetEvent manualResetEvent = null;

        private Mock<TaskRouterClient> mockClient;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<TaskRouterClient>(Credentials.AccountSid, Credentials.AuthToken);
            mockClient.CallBase = true;
        }

        [Test]
        public void ShouldGetReservation()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<Reservation>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new Reservation());
            var client = mockClient.Object;

            client.GetReservation(WORKSPACE_SID, TASK_SID, RESERVATION_SID);

            mockClient.Verify(trc => trc.Execute<Reservation>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/Tasks/{TaskSid}/Reservations/{ReservationSid}", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var taskSidParam = savedRequest.Parameters.Find(x => x.Name == "TaskSid");
            Assert.IsNotNull(taskSidParam);
            Assert.AreEqual(TASK_SID, taskSidParam.Value);
            var reservationSidParam = savedRequest.Parameters.Find(x => x.Name == "ReservationSid");
            Assert.IsNotNull(reservationSidParam);
            Assert.AreEqual(RESERVATION_SID, reservationSidParam.Value);
        }

        [Test]
        public void ShouldGetReservationAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<Reservation>(It.IsAny<RestRequest>(), It.IsAny<Action<Reservation>>()))
                .Callback<RestRequest, Action<Reservation>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);

            client.GetReservation(WORKSPACE_SID, TASK_SID, RESERVATION_SID, reservation => {
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<Reservation>(It.IsAny<RestRequest>(), It.IsAny<Action<Reservation>>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/Tasks/{TaskSid}/Reservations/{ReservationSid}", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(3, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var taskSidParam = savedRequest.Parameters.Find(x => x.Name == "TaskSid");
            Assert.IsNotNull(taskSidParam);
            Assert.AreEqual(TASK_SID, taskSidParam.Value);
            var reservationSidParam = savedRequest.Parameters.Find(x => x.Name == "ReservationSid");
            Assert.IsNotNull(reservationSidParam);
            Assert.AreEqual(RESERVATION_SID, reservationSidParam.Value);
        }

        [Test]
        public void ShouldListReservations()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<ReservationResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new ReservationResult());
            var client = mockClient.Object;

            client.ListReservations(WORKSPACE_SID, TASK_SID);

            mockClient.Verify(trc => trc.Execute<ReservationResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/Tasks/{TaskSid}/Reservations", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var taskSidParam = savedRequest.Parameters.Find(x => x.Name == "TaskSid");
            Assert.IsNotNull(taskSidParam);
            Assert.AreEqual(TASK_SID, taskSidParam.Value);
        }

        [Test]
        public void ShouldListReservationsAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<ReservationResult>(It.IsAny<RestRequest>(), It.IsAny<Action<ReservationResult>>()))
                .Callback<RestRequest, Action<ReservationResult>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);

            client.ListReservations(WORKSPACE_SID, TASK_SID, reservations => {
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<ReservationResult>(It.IsAny<RestRequest>(), It.IsAny<Action<ReservationResult>>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/Tasks/{TaskSid}/Reservations", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(2, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var taskSidParam = savedRequest.Parameters.Find(x => x.Name == "TaskSid");
            Assert.IsNotNull(taskSidParam);
            Assert.AreEqual(TASK_SID, taskSidParam.Value);
        }

        [Test]
        public void ShouldListReservationsUsingFilters()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<ReservationResult>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new ReservationResult());
            var client = mockClient.Object;

            client.ListReservations(WORKSPACE_SID, TASK_SID, "status", "assignmentStatus", "afterSid", "beforeSid", 10);

            mockClient.Verify(trc => trc.Execute<ReservationResult>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/Tasks/{TaskSid}/Reservations", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(7, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var taskSidParam = savedRequest.Parameters.Find(x => x.Name == "TaskSid");
            Assert.IsNotNull(taskSidParam);
            Assert.AreEqual(TASK_SID, taskSidParam.Value);
            var statusParam = savedRequest.Parameters.Find(x => x.Name == "Status");
            Assert.IsNotNull(statusParam);
            Assert.AreEqual("status", statusParam.Value);
            var assignmentStatusParam = savedRequest.Parameters.Find(x => x.Name == "AssignmentStatus");
            Assert.IsNotNull(assignmentStatusParam);
            Assert.AreEqual("assignmentStatus", assignmentStatusParam.Value);
            var afterSidParam = savedRequest.Parameters.Find(x => x.Name == "AfterSid");
            Assert.IsNotNull(afterSidParam);
            Assert.AreEqual("afterSid", afterSidParam.Value);
            var beforeSidParam = savedRequest.Parameters.Find(x => x.Name == "BeforeSid");
            Assert.IsNotNull(beforeSidParam);
            Assert.AreEqual("beforeSid", beforeSidParam.Value);
            var countSidParam = savedRequest.Parameters.Find(x => x.Name == "PageSize");
            Assert.IsNotNull(countSidParam);
            Assert.AreEqual(10, countSidParam.Value);
        }

        [Test]
        public void ShouldListReservationsUsingFiltersAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<ReservationResult>(It.IsAny<RestRequest>(), It.IsAny<Action<ReservationResult>>()))
                .Callback<RestRequest, Action<ReservationResult>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);

            client.ListReservations(WORKSPACE_SID, TASK_SID, "status", "assignmentStatus", "afterSid", "beforeSid", 10, reservations => {
                manualResetEvent.Set();
            });

            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<ReservationResult>(It.IsAny<RestRequest>(), It.IsAny<Action<ReservationResult>>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/Tasks/{TaskSid}/Reservations", savedRequest.Resource);
            Assert.AreEqual("GET", savedRequest.Method);
            Assert.AreEqual(7, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var taskSidParam = savedRequest.Parameters.Find(x => x.Name == "TaskSid");
            Assert.IsNotNull(taskSidParam);
            Assert.AreEqual(TASK_SID, taskSidParam.Value);
            var statusParam = savedRequest.Parameters.Find(x => x.Name == "Status");
            Assert.IsNotNull(statusParam);
            Assert.AreEqual("status", statusParam.Value);
            var assignmentStatusParam = savedRequest.Parameters.Find(x => x.Name == "AssignmentStatus");
            Assert.IsNotNull(assignmentStatusParam);
            Assert.AreEqual("assignmentStatus", assignmentStatusParam.Value);
            var afterSidParam = savedRequest.Parameters.Find(x => x.Name == "AfterSid");
            Assert.IsNotNull(afterSidParam);
            Assert.AreEqual("afterSid", afterSidParam.Value);
            var beforeSidParam = savedRequest.Parameters.Find(x => x.Name == "BeforeSid");
            Assert.IsNotNull(beforeSidParam);
            Assert.AreEqual("beforeSid", beforeSidParam.Value);
            var countSidParam = savedRequest.Parameters.Find(x => x.Name == "PageSize");
            Assert.IsNotNull(countSidParam);
            Assert.AreEqual(10, countSidParam.Value);
        }

        [Test]
        public void ShouldUpdateReservation()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.Execute<Reservation>(It.IsAny<RestRequest>()))
                .Callback<RestRequest>((request) => savedRequest = request)
                .Returns(new Reservation());
            var client = mockClient.Object;

            client.UpdateReservation(WORKSPACE_SID, TASK_SID, RESERVATION_SID, "reservationStatus", "WA123");

            mockClient.Verify(trc => trc.Execute<Reservation>(It.IsAny<RestRequest>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/Tasks/{TaskSid}/Reservations/{ReservationSid}", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(5, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var taskSidParam = savedRequest.Parameters.Find(x => x.Name == "TaskSid");
            Assert.IsNotNull(taskSidParam);
            Assert.AreEqual(TASK_SID, taskSidParam.Value);
            var reservationSidParam = savedRequest.Parameters.Find(x => x.Name == "ReservationSid");
            Assert.IsNotNull(reservationSidParam);
            Assert.AreEqual(RESERVATION_SID, reservationSidParam.Value);
            var reservationStatusParam = savedRequest.Parameters.Find(x => x.Name == "ReservationStatus");
            Assert.IsNotNull(reservationStatusParam);
            Assert.AreEqual("reservationStatus", reservationStatusParam.Value);
            var workerActivitySidParam = savedRequest.Parameters.Find(x => x.Name == "WorkerActivitySid");
            Assert.IsNotNull(workerActivitySidParam);
            Assert.AreEqual("WA123", workerActivitySidParam.Value);
        }

        [Test]
        public void ShouldUpdateReservationAsynchronously()
        {
            RestRequest savedRequest = null;
            mockClient.Setup(trc => trc.ExecuteAsync<Reservation>(It.IsAny<RestRequest>(), It.IsAny<Action<Reservation>>()))
                .Callback<RestRequest, Action<Reservation>>((request, action) => savedRequest = request);
            var client = mockClient.Object;
            manualResetEvent = new ManualResetEvent(false);

            client.UpdateReservation(WORKSPACE_SID, TASK_SID, RESERVATION_SID, "reservationStatus", "WA123", reservation => {
                manualResetEvent.Set();
            });
            manualResetEvent.WaitOne(1);

            mockClient.Verify(trc => trc.ExecuteAsync<Reservation>(It.IsAny<RestRequest>(), It.IsAny<Action<Reservation>>()), Times.Once);
            Assert.IsNotNull(savedRequest);
            Assert.AreEqual("Workspaces/{WorkspaceSid}/Tasks/{TaskSid}/Reservations/{ReservationSid}", savedRequest.Resource);
            Assert.AreEqual("POST", savedRequest.Method);
            Assert.AreEqual(5, savedRequest.Parameters.Count);
            var workspaceSidParam = savedRequest.Parameters.Find(x => x.Name == "WorkspaceSid");
            Assert.IsNotNull(workspaceSidParam);
            Assert.AreEqual(WORKSPACE_SID, workspaceSidParam.Value);
            var taskSidParam = savedRequest.Parameters.Find(x => x.Name == "TaskSid");
            Assert.IsNotNull(taskSidParam);
            Assert.AreEqual(TASK_SID, taskSidParam.Value);
            var reservationSidParam = savedRequest.Parameters.Find(x => x.Name == "ReservationSid");
            Assert.IsNotNull(reservationSidParam);
            Assert.AreEqual(RESERVATION_SID, reservationSidParam.Value);
            var reservationStatusParam = savedRequest.Parameters.Find(x => x.Name == "ReservationStatus");
            Assert.IsNotNull(reservationStatusParam);
            Assert.AreEqual("reservationStatus", reservationStatusParam.Value);
            var workerActivitySidParam = savedRequest.Parameters.Find(x => x.Name == "WorkerActivitySid");
            Assert.IsNotNull(workerActivitySidParam);
            Assert.AreEqual("WA123", workerActivitySidParam.Value);
        }
    }
}

